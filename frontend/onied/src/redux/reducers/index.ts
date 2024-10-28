import { combineReducers } from "redux";
import { CourseHierarchyReducer } from "./courseHierarchyReducer";
import { ChatsReducer } from "./chatReducer";

export const rootReducer = combineReducers({
  hierarchy: CourseHierarchyReducer,
  chats: ChatsReducer
});
