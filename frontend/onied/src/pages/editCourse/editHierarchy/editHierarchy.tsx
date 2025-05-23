import { useState, useEffect } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import api from "../../../config/axios";
import classes from "./editHierarchy.module.css";
import Button from "../../../components/general/button/button";
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
import NotFound from "../../../components/general/responses/notFound/notFound";
import Forbid from "../../../components/general/responses/forbid/forbid";
import CustomBeatLoader from "../../../components/general/customBeatLoader";
import { BlockType } from "../../../types/block";

type Block = {
  id: number;
  index: number;
  title: string;
  blockType: BlockType;
};

type Module = {
  id: number;
  index: number;
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
  const [expandedModules, setExpandedModules] = useState<Array<number>>([]);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [openedMenus, setOpenedMenus] = useState<Array<number>>([]);
  const notFound = <NotFound>Курс не найден.</NotFound>;
  const [isForbid, setIsForbid] = useState(false);
  const forbid = <Forbid>Вы не можете редактировать данный курс.</Forbid>;
  const id = Number(courseId);
  const blockIcons = [
    <></>,
    <FontAwesomeIcon icon={faFileLines} aria-label="конспект" />,
    <FontAwesomeIcon icon={faVideo} aria-label="видео" />,
    <FontAwesomeIcon icon={faListCheck} aria-label="задания" />,
  ];

  const handleErrors = (error: any) => {
    if ("response" in error && error.response.status == 404) {
      setHierarchy(null);
    } else if ("response" in error && error.response.status == 403) {
      setHierarchy(null);
      setIsForbid(true);
    }
  };

  const deleteBlock = (moduleIndex: number, blockId: number) => {
    const newArray = Array.from(hierarchy!.modules);
    const blockIndex = newArray[moduleIndex].blocks.findIndex(
      (block) => block.id == blockId
    );

    if (blockIndex == -1) return;
    newArray[moduleIndex].blocks.splice(blockIndex, 1);
    newArray[moduleIndex].blocks.forEach(
      (block, index) => (block.index = index)
    );
    setHierarchy({ ...hierarchy!, modules: newArray });
    api
      .delete("courses/" + courseId + "/edit/delete-block/?blockId=" + blockId)
      .catch((error) => handleErrors(error));
  };

  const deleteModule = (moduleId: number) => {
    const newArray = Array.from(hierarchy!.modules);
    const moduleIndex = newArray.findIndex((module) => module.id == moduleId);
    if (moduleIndex == -1) return;
    newArray.splice(moduleIndex, 1);
    newArray.forEach((module, index) => (module.index = index));
    setHierarchy({ ...hierarchy!, modules: newArray });
    api
      .delete(
        "courses/" + courseId + "/edit/delete-module?moduleId=" + moduleId
      )
      .catch((error) => handleErrors(error));
  };

  const addModule = () => {
    const newArray = Array.from(hierarchy!.modules);
    api
      .post("courses/" + courseId + "/edit/add-module")
      .then((response) => {
        newArray.push({
          id: response.data,
          index: newArray.length,
          blocks: [],
          title: "Новый модуль",
        });
        setHierarchy({ ...hierarchy!, modules: newArray });
      })
      .catch((error) => handleErrors(error));
  };

  const addBlock = (moduleId: number, blockType: number) => {
    const newArray = Array.from(hierarchy!.modules);
    const moduleIndex = newArray.findIndex((module) => module.id == moduleId);
    api
      .post("courses/" + courseId + "/edit/add-block/" + moduleId, {
        blockType,
      })
      .then((response) => {
        newArray[moduleIndex].blocks.push({
          id: response.data,
          index: newArray[moduleIndex].blocks.length,
          title: "Новый блок",
          blockType: blockType,
        });
        setHierarchy({ ...hierarchy!, modules: newArray });
      })
      .catch((error) => handleErrors(error));

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

  const sendNameBlock = (blockId: number, value: string) => {
    api
      .put("courses/" + courseId + "/edit/rename-block", {
        blockId: blockId,
        title: value,
      })
      .catch((error) => handleErrors(error));
  };

  const renameModule = (moduleId: number, value: string) => {
    const newArray = Array.from(hierarchy!.modules);
    const moduleIndex = newArray.findIndex((module) => module.id == moduleId);
    if (moduleIndex == -1) return;
    newArray[moduleIndex].title = value;
    setHierarchy({ ...hierarchy!, modules: newArray });
  };

  const sendNameModule = (moduleId: number, value: string) => {
    api
      .put("courses/" + courseId + "/edit/rename-module", {
        moduleId: moduleId,
        title: value,
      })
      .catch((error) => handleErrors(error));
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
      newArray.forEach((module, index) => (module.index = index));
      setHierarchy({ ...hierarchy!, modules: newArray });
      api
        .put("courses/" + courseId + "/edit/hierarchy", {
          ...hierarchy!,
          modules: newArray,
        })
        .catch((error) => handleErrors(error));
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
        newArrayModules[fromModule].blocks.forEach(
          (block, index) => (block.index = index)
        );
        newArrayModules[toModule].blocks.push(removed);
        newArrayModules[toModule].blocks.forEach(
          (block, index) => (block.index = index)
        );
        setHierarchy({ ...hierarchy!, modules: newArrayModules });
        api
          .put("courses/" + courseId + "/edit/hierarchy", {
            ...hierarchy!,
            modules: newArrayModules,
          })
          .catch((error) => handleErrors(error));
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
        newArrayModules[fromModule].blocks.forEach(
          (block, index) => (block.index = index)
        );
        newArrayModules[toModule].blocks.splice(
          result.destination.index,
          0,
          removed
        );
        newArrayModules[toModule].blocks.forEach(
          (block, index) => (block.index = index)
        );
        setHierarchy({ ...hierarchy!, modules: newArrayModules });
        api
          .put("courses/" + courseId + "/edit/hierarchy", {
            ...hierarchy!,
            modules: newArrayModules,
          })
          .catch((error) => handleErrors(error));
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
            aria-label="блок"
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
                onBlur={(event) => sendNameBlock(block.id, event.target.value)}
                placeholder="название блока"
              />
              <span className={classes.blockButtons}>
                <Link
                  className={classes.blockButton}
                  to={new URL("" + block.id, window.location.href).toString()}
                >
                  <FontAwesomeIcon
                    icon={faPencil}
                    aria-label="редактировать блок"
                  />
                </Link>
                <a
                  onClick={() => deleteBlock(moduleIndex, block.id)}
                  className={classes.blockButton}
                >
                  <FontAwesomeIcon icon={faTrash} aria-label="удалить блок" />
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
            aria-label="модуль"
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
                          aria-label={
                            expandedModules.includes(module.id)
                              ? "скрыть"
                              : "раскрыть"
                          }
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
                          onBlur={(event) => {
                            sendNameModule(module.id, event.target.value);
                          }}
                          placeholder="название модуля"
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
                            <FontAwesomeIcon
                              icon={faPlus}
                              aria-label="добавить блок"
                            />
                          </a>
                          <Menu
                            open={openedMenus.includes(module.id)}
                            anchorEl={anchorEl}
                            onClose={() => {
                              setOpenedMenus([]);
                              setAnchorEl(null);
                            }}
                          >
                            <MenuItem
                              onClick={() =>
                                addBlock(module.id, BlockType.SummaryBlock)
                              }
                            >
                              Конспект
                            </MenuItem>
                            <MenuItem
                              onClick={() =>
                                addBlock(module.id, BlockType.VideoBlock)
                              }
                            >
                              Видео
                            </MenuItem>
                            <MenuItem
                              onClick={() =>
                                addBlock(module.id, BlockType.TasksBlock)
                              }
                            >
                              Задания
                            </MenuItem>
                          </Menu>
                          <a
                            onClick={() => deleteModule(module.id)}
                            className={classes.moduleButton}
                          >
                            <FontAwesomeIcon
                              icon={faTrash}
                              aria-label="удалить модуль"
                            />
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
      .get("courses/" + courseId + "/edit/check-edit-course")
      .then((_) => {
        api
          .get("courses/" + id + "/hierarchy/")
          .then((response) => {
            console.log(response.data);
            if ("modules" in response.data) {
              response.data.modules.sort((a: Module, b: Module) =>
                a.index > b.index ? 1 : -1
              );
              response.data.modules.forEach((module: Module) => {
                if ("blocks" in module)
                  module.blocks.sort((a, b) => (a.index > b.index ? 1 : -1));
              });
            }
            setHierarchy(response.data);
          })
          .catch((error) => {
            console.log(error);

            if (error.response?.status == 404) setHierarchy(null);
            if (error.response?.status === 403) setIsForbid(true);
          });
      })
      .catch((error) => {
        if (error.response?.status === 403) setIsForbid(true);
        if (error.response?.status == 404) setHierarchy(null);
      });
  }, [id]);

  if (isNaN(id)) {
    console.log(id);
    console.log(courseId);
    return notFound;
  }

  if (isForbid) return forbid;

  if (hierarchy === undefined) return <CustomBeatLoader />;
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
