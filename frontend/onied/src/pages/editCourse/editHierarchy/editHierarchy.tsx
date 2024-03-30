import { useState, useEffect, useCallback } from "react";
import { Link, useParams } from "react-router-dom";
import api from "../../../config/axios";
import classes from "./editHierarchy.module.css";
import Button from "../../../components/general/button/button";
import { BeatLoader } from "react-spinners";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import type { DropResult, DragStart } from "@hello-pangea/dnd";

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
  const { courseId } = useParams();
  const [hierarchy, setHierarchy] = useState<Course | null | undefined>();
  const [moduleDropDisabled, setModuleDropDisabled] = useState(false);
  const [hierarchyDropDisabled, setHierarchyDropDisabled] = useState(false);
  const notFound = <h1 style={{ margin: "3rem" }}>Курс не найден.</h1>;
  const id = Number(courseId);
  const blockTypes = ["", "summary", "video", "tasks"];

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
      if (!result.destination.droppableId.startsWith("module")) {
        return;
      }
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
      console.log(newArrayModules);
      setHierarchy({ ...hierarchy!, modules: newArrayModules });
    }
  };

  const renderBlock = (block: Block, index: number, moduleIndex: number) => {
    return (
      <Draggable key={block.id} draggableId={"block" + block.id} index={index}>
        {(provided, snapshot) => (
          <div
            className={classes.block}
            ref={provided.innerRef}
            {...provided.draggableProps}
            {...provided.dragHandleProps}
          >
            {moduleIndex + 1}.{index + 1}. {block.title}
          </div>
        )}
      </Draggable>
    );
  };

  const renderModule = (module: Module, index: number) => {
    return (
      <div className={classes.moduleContainer} key={module.id}>
        <Draggable draggableId={"module" + module.id} index={index}>
          {(provided: any, snapshot: any) => (
            <div
              className={classes.module}
              ref={provided.innerRef}
              {...provided.draggableProps}
              {...provided.dragHandleProps}
            >
              {index + 1}. {module.title}
            </div>
          )}
        </Draggable>
        <Droppable
          droppableId={"module" + module.id}
          isDropDisabled={moduleDropDisabled}
        >
          {(provided, snapshot) => (
            <div
              {...provided.droppableProps}
              ref={provided.innerRef}
              className={classes.blockList}
            >
              {module.blocks.map((block, bi) => renderBlock(block, bi, index))}
              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </div>
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
    <div className={classes.container}>
      <div className={classes.pageHeaderContainer}>
        <Link to={"/courses/" + courseId + "/edit"} className={classes.back}>
          ⟵ к редактированию превью
        </Link>
        <h1 className={classes.title}>{hierarchy!.title}</h1>
      </div>
      <div className={classes.pageBodyContainer}>
        <div className={classes.buttonsUp}>
          <Button>добавить модуль</Button>
        </div>
        <DragDropContext onDragEnd={onDragEnd} onDragStart={onDragStart}>
          <Droppable
            droppableId="hierarchy"
            isDropDisabled={hierarchyDropDisabled}
          >
            {(provided, snapshot) => (
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
        <div className={classes.buttonsDown}>
          <Button>сохранить</Button>
        </div>
      </div>
    </div>
  );
}
export default EditCourseHierarchy;
