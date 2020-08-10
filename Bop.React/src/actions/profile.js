import * as types from "../actionTypes/profile";
import { profileService } from "./../services/profile";
import { isRequesting, isRequested, errorOccurred } from "./request";

export const profileFetched = (profile) => ({
  type: types.ProfileFetched,
  payload: profile,
});

export const cardsFetched = (cards) => ({
  type: types.CardsFetched,
  payload: cards,
});

export const getProfile = () => (dispatch) => {
  dispatch(isRequesting());
  profileService
    .getProfile()
    .then(async (result) => {
      await dispatch(profileFetched(result.data.data));
      dispatch(isRequested());
    })
    .catch((error) => {
      dispatch(errorOccurred());
    });
};

export const updateProfile = (model) => (dispatch) => {
  dispatch(isRequesting());
  profileService
    .updateProfile(model)
    .then(async (result) => {
      await dispatch(getProfile());
      dispatch(isRequested(result.data.meta.message));
    })
    .catch((error) => {});
};
