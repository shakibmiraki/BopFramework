import * as types from "../actionTypes/request";
import { toastService } from "./../services/toast";

const initialState = {
  isRequesting: false,
  isRequested: false,
};

export default function requestReducer(state = initialState, action) {
  if (action.type === types.IsRequesting) {
    return {
      ...state,
      isRequesting: true,
      isRequested: false,
    };
  }
  if (action.type === types.IsRequested) {
    toastService.notifies(action.payload);
    return {
      ...state,
      isRequesting: false,
      isRequested: true,
    };
  }
  if (action.type === types.ErrorOccurred) {
    return {
      ...state,
      isRequesting: false,
      isRequested: true,
    };
  }
  return state;
}
