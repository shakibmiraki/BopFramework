import * as types from "../actionTypes/theme";

const initialState = {
  name: null,
};

export default function themeReducer(state = initialState, action) {
  if (action.type === types.SetActiveTheme) {
    return {
      name: action.payload,
    };
  }
  return state;
}
