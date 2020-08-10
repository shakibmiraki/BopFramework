//react
import React from "react";
import ReactDOM from "react-dom";

import App from "./App";
import AppProviders from "./context";

//redux
import { Provider } from "react-redux";
import { applyMiddleware, compose, createStore } from "redux";
import { cacheEnhancer } from "redux-cache";
import thunk from "redux-thunk";
import reducer from "./reducers";

//service worker
import * as serviceWorker from "./serviceWorker";

//bootstrap
import "bootstrap/dist/css/bootstrap.css";

//localization
import { I18nextProvider } from "react-i18next";
import i18n from "./i18n";

//route
import { BrowserRouter } from "react-router-dom";

//styles
import "./index.scss";

import PrintProvider from "react-easy-print";

const store = createStore(
  reducer,
  compose(
    applyMiddleware(thunk),
    cacheEnhancer(),
    (window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ && window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__()) || compose
  )
);

ReactDOM.render(
  <BrowserRouter>
    <I18nextProvider i18n={i18n}>
      <Provider store={store}>
        <AppProviders>
          <PrintProvider>
            <App />
          </PrintProvider>
        </AppProviders>
      </Provider>
    </I18nextProvider>
  </BrowserRouter>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
if (process.env.NODE_ENV === "production") {
  serviceWorker.register();
} else {
  serviceWorker.unregister();
}
