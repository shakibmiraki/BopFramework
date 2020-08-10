import * as types from "../actionTypes/counter";

const initialState = {
  timer: 0,
};

export default function counterReducer(state = initialState, action) {
  if (action.type === types.SetCounter) {
    return {
      ...state,
      timer: action.payload,
    };
  }
  return state;
}
