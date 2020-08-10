/** @jsx jsx */
import { jsx, css } from "@emotion/core";

export const Dashed = ({borderThick}) => {
  return <div css={css({
    width: "100%",
    borderBottom: `${borderThick}px dashed #c3c3c3`,
  })}></div>;
};
