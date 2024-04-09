import { combineReducers } from "redux";
import { CourseHierarchyReducer } from "./courseHierarchyReducer";

export const rootReducer = combineReducers({
  hierarchy: CourseHierarchyReducer,
});
