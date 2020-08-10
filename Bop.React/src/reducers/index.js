import { combineReducers } from "redux";

//import custom reducer
import languageReducer from "./language";
import themeReducer from "./theme";
import authReducer from "./auth";
import profileReducer from "./profile";
import requestReducer from "./request";
import appReducer from "./app";
import counterReducer from "./counter";

const rootReducer = combineReducers({
  language: languageReducer,
  theme: themeReducer,
  auth: authReducer,
  profile: profileReducer,
  request: requestReducer,
  app: appReducer,
  counter: counterReducer,
});

export default rootReducer;
