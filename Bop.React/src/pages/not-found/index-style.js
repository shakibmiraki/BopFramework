import { css } from "@emotion/core";

const styles = css({
  ".error-holder .error-text": {
    fontSize: "45px",
    letterSpacing: "-1.5px",
    marginTop: "5px",
    textTransform: "uppercase",
    lineHeight: "55px",
  },
  ".error-holder .box": {
    display: "flex",
    margin: "0 auto",
    width: "285px",
    height: "285px",
    position: "relative",
  },
  ".error-holder": {
    width: "100%",
    background: "#fff",
    margin: "10px 0",
    textAlign: "center",
  },
  ".error-holder .box .error-type": {
    fontSize: "156px",
    marginBottom: 0,
    color: "#ffffff",
    letterSpacing: "-18px",
    flex: "1",
    alignSelf: "center",
  },
});

export default styles;
