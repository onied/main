import { useState, useEffect } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import api from "../../../config/axios";
import classes from "./editHierarchy.module.css";
import Button from "../../../components/general/button/button";
import { BeatLoader } from "react-spinners";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { StyledEngineProvider } from "@mui/material/styles";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faVideo,
  faFileLines,
  faListCheck,
  faPencil,
  faTrash,
  faPlus,
} from "@fortawesome/free-solid-svg-icons";
import type { DropResult, DragStart } from "@hello-pangea/dnd";
import { Menu, MenuItem } from "@mui/material";
import ButtonGoBack from "../../../components/general/buttonGoBack/buttonGoBack";

type Block = {
  id: number;
  title: string;
  blockType: number;
};

type Module = {
  id: number;
  title: string;
  blocks: Array<Block>;
};

type Course = {
  id: number;
  modules: Array<Module>;
  title: string;
};

function EditCourseHierarchy() {
  const navigator = useNavigate();
  const { courseId } = useParams();
  const [hierarchy, setHierarchy] = useState<Course | null | undefined>();
  const [moduleDropDisabled, setModuleDropDisabled] = useState(false);
  const [hierarchyDropDisabled, setHierarchyDropDisabled] = useState(false);
  const [createdModulesCounter, setCreatedModulesCounter] = useState(1);
  const [createdBlocksCounter, setCreatedBlocksCounter] = useState(1);
  const [expandedModules, setExpandedModules] = useState<Array<number>>([]);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [openedMenus, setOpenedMenus] = useState<Array<number>>([]);
  const notFound = <h1 style={{ margin: "3rem" }}>Курс не найден.</h1>;
  const id = Number(courseId);
  const blockTypes = [<></>, "summary", "video", "tasks"];
  const blockIcons = [
    <></>,
    <FontAwesomeIcon icon={faFileLines} />,
    <FontAwesomeIcon icon={faVideo} />,
    <FontAwesomeIcon icon={faListCheck} />,
  ];

  const deleteBlock = (moduleIndex: number, blockId: number) => {
    const newArray = Array.from(hierarchy!.modules);
    const blockIndex = newArray[moduleIndex].blocks.findIndex(
      (block) => block.id == blockId
    );
    if (blockIndex == -1) return;
    newArray[moduleIndex].blocks.splice(blockIndex, 1);
    setHierarchy({ ...hierarchy!, modules: newArray });
  };

  const deleteModule = (moduleId: number) => {
    const newArray = Array.from(hierarchy!.modules);
    const moduleIndex = newArray.findIndex((module) => module.id == moduleId);
    if (moduleIndex == -1) return;
    newArray.splice(moduleIndex, 1);
    setHierarchy({ ...hierarchy!, modules: newArray });
  };

  const addModule = () => {
    const newArray = Array.from(hierarchy!.modules);
    newArray.push({
      id: -createdModulesCounter,
      blocks: [],
      title: "Новый модуль",
    });
    setCreatedModulesCounter(createdModulesCounter + 1);
    setHierarchy({ ...hierarchy!, modules: newArray });
  };

  const addBlock = (moduleId: number, blockType: number) => {
    const newArray = Array.from(hierarchy!.modules);
    const moduleIndex = newArray.findIndex((module) => module.id == moduleId);
    newArray[moduleIndex].blocks.push({
      id: -createdBlocksCounter,
      title: "Новый блок",
      blockType: blockType,
    });
    setCreatedBlocksCounter(createdBlocksCounter + 1);
    setHierarchy({ ...hierarchy!, modules: newArray });
    const exp = Array.from(expandedModules);
    if (!exp.includes(moduleId)) exp.push(moduleId);
    setExpandedModules(exp);
    setOpenedMenus([]);
    setAnchorEl(null);
  };

  const renameBlock = (moduleIndex: number, blockId: number, value: string) => {
    const newArray = Array.from(hierarchy!.modules);
    const blockIndex = newArray[moduleIndex].blocks.findIndex(
      (block) => block.id == blockId
    );
    if (blockIndex == -1) return;
    newArray[moduleIndex].blocks[blockIndex].title = value;
    setHierarchy({ ...hierarchy!, modules: newArray });
  };

  const renameModule = (moduleId: number, value: string) => {
    const newArray = Array.from(hierarchy!.modules);
    const moduleIndex = newArray.findIndex((module) => module.id == moduleId);
    if (moduleIndex == -1) return;
    newArray[moduleIndex].title = value;
    setHierarchy({ ...hierarchy!, modules: newArray });
  };

  const onDragStart = (start: DragStart) => {
    setModuleDropDisabled(start.draggableId.startsWith("module"));
    setHierarchyDropDisabled(start.draggableId.startsWith("block"));
  };

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) {
      return;
    }
    if (result.draggableId.startsWith("module")) {
      if (result.destination.droppableId != "hierarchy") {
        return;
      }
      const newArray = Array.from(hierarchy!.modules);
      const [removed] = newArray.splice(result.source.index, 1);
      newArray.splice(result.destination.index, 0, removed);
      setHierarchy({ ...hierarchy!, modules: newArray });
    } else if (result.draggableId.startsWith("block")) {
      if (result.destination.droppableId.startsWith("combineModule")) {
        const newArrayModules = Array.from(hierarchy!.modules);
        const fromModule = newArrayModules.findIndex(
          (value) =>
            value.id == Number(result.source.droppableId.replace("module", ""))
        );
        const toModule = newArrayModules.findIndex(
          (value) =>
            value.id ==
            Number(result.destination!.droppableId.replace("combineModule", ""))
        );
        const [removed] = newArrayModules[fromModule].blocks.splice(
          result.source.index,
          1
        );
        newArrayModules[toModule].blocks.push(removed);
        setHierarchy({ ...hierarchy!, modules: newArrayModules });
      }
      if (result.destination.droppableId.startsWith("module")) {
        const newArrayModules = Array.from(hierarchy!.modules);
        const fromModule = newArrayModules.findIndex(
          (value) =>
            value.id == Number(result.source.droppableId.replace("module", ""))
        );
        const toModule = newArrayModules.findIndex(
          (value) =>
            value.id ==
            Number(result.destination!.droppableId.replace("module", ""))
        );
        const [removed] = newArrayModules[fromModule].blocks.splice(
          result.source.index,
          1
        );
        newArrayModules[toModule].blocks.splice(
          result.destination.index,
          0,
          removed
        );
        setHierarchy({ ...hierarchy!, modules: newArrayModules });
      }
    }
  };

  const renderBlock = (block: Block, index: number, moduleIndex: number) => {
    return (
      <Draggable key={block.id} draggableId={"block" + block.id} index={index}>
        {(provided, _) => (
          <div
            className={classes.block}
            ref={provided.innerRef}
            {...provided.draggableProps}
            {...provided.dragHandleProps}
          >
            <div className={classes.blockHeader}>
              <span className={classes.blockIndex}>
                {moduleIndex + 1}.{index + 1}.
              </span>
              <input
                type="text"
                className={classes.blockTitle}
                value={block.title}
                onChange={(event) =>
                  renameBlock(moduleIndex, block.id, event.target.value)
                }
              />
              <span className={classes.blockButtons}>
                <Link
                  className={classes.blockButton}
                  to={new URL(
                    "" + block.id + "/" + blockTypes[block.blockType],
                    window.location.href
                  ).toString()}
                >
                  <FontAwesomeIcon icon={faPencil} />
                </Link>
                <a
                  onClick={() => deleteBlock(moduleIndex, block.id)}
                  className={classes.blockButton}
                >
                  <FontAwesomeIcon icon={faTrash} />
                </a>
              </span>
            </div>
            <div className={classes.blockType}>
              {blockIcons[block.blockType]}
            </div>
          </div>
        )}
      </Draggable>
    );
  };

  const renderModule = (module: Module, index: number) => {
    return (
      <Draggable
        draggableId={"module" + module.id}
        index={index}
        key={module.id}
      >
        {(provided, _) => (
          <div
            className={classes.moduleContainer}
            ref={provided.innerRef}
            {...provided.draggableProps}
            {...provided.dragHandleProps}
          >
            <Accordion
              disableGutters
              elevation={0}
              className={classes.accordion}
              expanded={expandedModules.includes(module.id)}
            >
              <Droppable
                droppableId={"combineModule" + module.id}
                isDropDisabled={moduleDropDisabled}
              >
                {(provided, _) => (
                  <div {...provided.droppableProps} ref={provided.innerRef}>
                    <AccordionSummary
                      expandIcon={
                        <ExpandMoreIcon
                          className={classes.expandMoreIcon}
                          onClick={() => {
                            const newArray = Array.from(expandedModules);
                            if (newArray.includes(module.id))
                              newArray.splice(
                                newArray.findIndex((m) => m == module.id),
                                1
                              );
                            else newArray.push(module.id);
                            setExpandedModules(newArray);
                          }}
                        />
                      }
                      className={classes.module}
                    >
                      <div className={classes.moduleHeader}>
                        <span className={classes.moduleIndex}>
                          {index + 1}.
                        </span>
                        <input
                          type="text"
                          className={classes.moduleTitle}
                          value={module.title}
                          onChange={(event) =>
                            renameModule(module.id, event.target.value)
                          }
                        />
                        <span className={classes.moduleButtons}>
                          <a
                            className={classes.moduleButton}
                            onClick={(event) => {
                              const newArray = Array.from(openedMenus);
                              newArray.push(module.id);
                              setOpenedMenus(newArray);
                              setAnchorEl(event.currentTarget);
                            }}
                          >
                            <FontAwesomeIcon icon={faPlus} />
                          </a>
                          <Menu
                            open={openedMenus.includes(module.id)}
                            anchorEl={anchorEl}
                            onClose={() => {
                              setOpenedMenus([]);
                              setAnchorEl(null);
                            }}
                          >
                            <MenuItem onClick={() => addBlock(module.id, 1)}>
                              Конспект
                            </MenuItem>
                            <MenuItem onClick={() => addBlock(module.id, 2)}>
                              Видео
                            </MenuItem>
                            <MenuItem onClick={() => addBlock(module.id, 3)}>
                              Задания
                            </MenuItem>
                          </Menu>
                          <a
                            onClick={() => deleteModule(module.id)}
                            className={classes.moduleButton}
                          >
                            <FontAwesomeIcon icon={faTrash} />
                          </a>
                        </span>
                      </div>
                    </AccordionSummary>
                    {provided.placeholder}
                  </div>
                )}
              </Droppable>

              <AccordionDetails className={classes.accordionDetails}>
                <Droppable
                  droppableId={"module" + module.id}
                  isDropDisabled={moduleDropDisabled}
                >
                  {(provided, _) => (
                    <div
                      {...provided.droppableProps}
                      ref={provided.innerRef}
                      className={classes.blockList}
                    >
                      {module.blocks.map((block, bi) =>
                        renderBlock(block, bi, index)
                      )}
                      {provided.placeholder}
                    </div>
                  )}
                </Droppable>
              </AccordionDetails>
            </Accordion>
          </div>
        )}
      </Draggable>
    );
  };

  useEffect(() => {
    if (isNaN(id)) {
      setHierarchy(null);
      return;
    }
    api
      .get("courses/" + id + "/hierarchy/")
      .then((response) => {
        console.log(response.data);
        setHierarchy(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setHierarchy(null);
        }
      });
  }, [id]);

  if (isNaN(id)) {
    console.log(id);
    console.log(courseId);
    return notFound;
  }

  if (hierarchy === undefined)
    return (
      <BeatLoader
        color="var(--accent-color)"
        style={{ margin: "3rem" }}
      ></BeatLoader>
    );
  if (hierarchy === null) return notFound;

  return (
    <StyledEngineProvider injectFirst>
      <div className={classes.container}>
        <div className={classes.pageHeaderContainer}>
          <ButtonGoBack
            onClick={() => navigator("../", { relative: "path" })}
            style={{ width: "fit-content" }}
          >
            ⟵ к редактированию превью
          </ButtonGoBack>
          <h1 className={classes.title}>{hierarchy!.title}</h1>
        </div>
        <div className={classes.pageBodyContainer}>
          <div className={classes.buttonsUp}>
            <Button onClick={addModule}>добавить модуль</Button>
          </div>
          <DragDropContext onDragEnd={onDragEnd} onDragStart={onDragStart}>
            <Droppable
              droppableId="hierarchy"
              isDropDisabled={hierarchyDropDisabled}
            >
              {(provided, _) => (
                <div
                  {...provided.droppableProps}
                  ref={provided.innerRef}
                  className={classes.moduleList}
                >
                  {hierarchy!.modules.map(renderModule)}
                  {provided.placeholder}
                </div>
              )}
            </Droppable>
          </DragDropContext>
        </div>
      </div>
    </StyledEngineProvider>
  );
}
export default EditCourseHierarchy;
