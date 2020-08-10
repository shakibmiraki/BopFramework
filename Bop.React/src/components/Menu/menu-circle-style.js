import { css } from "@emotion/core";

const styles = css({
  margin: "auto",
  ".image-size": {
    padding: "18px",
    margin: "auto",
    width: "82px",
    height: "82px",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    "@media (min-width: 375px)": {
      width: "89px",
      height: "89px",
    },
    "@media (min-width: 515px)": {
      width: "100px",
      height: "100px",
      padding:"22px"
    },

  },
  ".image-size img": {
    maxWidth: "100%",
    maxHeight: "100%",
  },
  a: {
    ":hover": {
      textDecoration: "none",
    },
  },
  "a span": {
    padding: "2px 0px",
    fontSize: "0.9rem",
    fontWeight: "500",
  },
});

export default styles;
