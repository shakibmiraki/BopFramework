import React from "react";
import Routing from "./components/Share/routes";
import { ToastContainer, Flip } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useSelector } from "react-redux";
import { config } from "./config";
import { FullPageSpinner } from "./components/Share/spinner";

const App = () => {
  const { ltr } = useSelector((state) => state.language);
  return (
    <React.Fragment>
      <Routing />
      <ToastContainer
        position="bottom-center"
        autoClose={config.toast_auto_close}
        transition={Flip}
        {...(!ltr && { rtl: true })}
      />
      <FullPageSpinner />
    </React.Fragment>
  );
};

export default App;
