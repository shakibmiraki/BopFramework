import * as request_types from "../actionTypes/request";

export const isRequesting = () => {
  return {
    type: request_types.IsRequesting,
  };
};

export const isRequested = (messages) => {
  return {
    type: request_types.IsRequested,
    payload: messages,
  };
};

export const errorOccurred = () => ({
  type: request_types.ErrorOccurred,
});
