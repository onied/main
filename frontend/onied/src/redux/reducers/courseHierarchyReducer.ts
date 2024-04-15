type CourseHierarchyState = {
  hierarchy: any;
};

const initialState: CourseHierarchyState = {
  hierarchy: undefined,
};

export enum CourseHierarchyActionTypes {
  FETCH_HIERARCHY = "FETCH_HIERARCHY",
  FETCH_HIERARCHY_SUCCESS = "FETCH_HIERARCHY_SUCCESS",
  FETCH_HIERARCHY_ERROR = "FETCH_HIERARCHY_ERROR",
}

interface FetchHierarchyAction {
  type: CourseHierarchyActionTypes.FETCH_HIERARCHY;
}

interface FetchHierarchySuccessAction {
  type: CourseHierarchyActionTypes.FETCH_HIERARCHY_SUCCESS;
  payload: any;
}

interface FetchHierarchyErrorAction {
  type: CourseHierarchyActionTypes.FETCH_HIERARCHY_ERROR;
  payload: null;
}

export type CourseHierarchyAction =
  | FetchHierarchyAction
  | FetchHierarchySuccessAction
  | FetchHierarchyErrorAction;

export const CourseHierarchyReducer = (
  state = initialState,
  action: CourseHierarchyAction
): CourseHierarchyState => {
  switch (action.type) {
    case CourseHierarchyActionTypes.FETCH_HIERARCHY:
      return { hierarchy: undefined };
    case CourseHierarchyActionTypes.FETCH_HIERARCHY_SUCCESS:
      return { hierarchy: action.payload };
    case CourseHierarchyActionTypes.FETCH_HIERARCHY_ERROR:
      return { hierarchy: {} };
    default:
      return state;
  }
};
