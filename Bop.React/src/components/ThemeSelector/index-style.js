import { css } from "@emotion/core";
import { global_config } from "./../../constants/constant";

const styles = css({
  ".image-size": {
    width: "80px",
    height: "80px",
    "> div": {
      height: "100%",
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
      fontSize: "35px",
    },
  },
  ".max-width": {
    maxWidth: global_config.circle_fix_width,
    margin: "auto",
  },
  img: {
    padding: "26%",
  },
});

export default styles;
