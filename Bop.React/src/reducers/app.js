import * as types from "../actionTypes/app";

const initialState = {
  categories: [],
};

export default function appReducer(state = initialState, action) {
  if (action.type === types.MenuFetched) {
    return {
      ...state,
      categories: action.payload,
    };
  }
  return state;
}
