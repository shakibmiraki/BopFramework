import * as types from "../actionTypes/auth";
import { signup_steps } from "./../models/steps/signup";

const initialState = {
  step: signup_steps.basic,
};

export default function authReducer(state = initialState, action) {
  if (action.type === types.SetStep) {
    return {
      ...state,
      step: action.payload,
    };
  }
  if (action.type === types.PreviousStep) {
    return {
      ...state,
      step: Math.max(state.step - 1, 0),
    };
  }

  return state;
}
