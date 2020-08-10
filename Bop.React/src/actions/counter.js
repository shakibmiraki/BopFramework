import * as types from "../actionTypes/counter";

export const setTimer = (seconds) => ({
  type: types.SetCounter,
  payload: seconds,
});
