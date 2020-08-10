const setKey = (keyName, keyValue) => {
  window.localStorage.setItem(keyName, keyValue);
};

const getKey = (keyName) => {
  return window.localStorage.getItem(keyName);
};

const removeKey = (keyName) => {
  window.localStorage.removeItem(keyName);
};

export const localStorageService = {
  setKey,
  getKey,
  removeKey,
};
