/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import styles from "./menu-circle-style";
import { NavLink } from "react-router-dom";

export const MenuCircle = ({ icon, text, url, onClick }) => {
  const path = url ? `${url}/` : "#";
  return (
    <div className="col-4 text-center mt-1 mb-1 p-0" css={css(styles)}>
      <NavLink to={path} className="d-block text-secondary" onClick={onClick}>
        <div className="border rounded-circle img-rounded image-size shadow-sm bg-white">{icon}</div>
        <span className="d-block">{text}</span>
      </NavLink>
    </div>
  );
};
