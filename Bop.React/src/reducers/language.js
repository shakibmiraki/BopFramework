import * as types from "../actionTypes/language";

const initialState = {
  languageCode: null,
  ltr: false,
  locale: "fa",
};

export default function languageReducer(state = initialState, action) {
  if (action.type === types.SetLanguage) {
    return {
      ...state,
      languageCode: action.payload.languageCode,
      ltr: action.payload.ltr,
      locale: action.payload.locale,
    };
  }
  return state;
}
