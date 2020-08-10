import { css } from "@emotion/core";

const menu_item_padding = "40%";
const border_radius = "30px";
const hamburger_icon_size = "32px";
const font_size = "14px";

const styles = css({
  ".bm-cross-button,.bm-cross-button *,.bm-burger-button,.bm-burger-button *": {
    outline: "none",
  },
  ".bm-burger-button": {
    position: "absolute",
    zIndex: "1 !important",
    top: "13px",
    width: hamburger_icon_size,
    right: "15px",
    svg: {
      fontSize: "32px",
    },
  },
  ".bm-burger-bars": {
    background: "#373a47",
  },
  ".bm-burger-bars-hover": {
    background: "#a90000",
  },
  ".bm-cross-button": {
    top: "18px !important",
    height: "24px",
    width: "24px",
    svg: {
      fontSize: "26px",
    },
  },
  ".right .bm-cross-button": {
    right: "unset !important",
    left: "18px !important",
  },
  ".left .bm-cross-button": {
    left: "unset !important",
    right: "18px !important",
  },
  "& .bm-menu-wrap": {
    position: "fixed",
    height: "100vh !important",
    top: 0,
    backgroundColor: "white !important",
  },
  "& .left .bm-menu-wrap": {
    left: 0,
    right: "unset !important",
  },
  "& .right .bm-menu-wrap": {
    left: "unset !important",
    right: 0,
  },
  "& .bm-menu": {
    padding: "50px 0",
    fontSize: "1.15em",
    width: "100%",
  },
  "& .bm-morph-shape": {
    fill: "#ffffff",
    width: "100% !important",
  },
  "& .bm-item-list": {
    color: "#b8b7ad",
    padding: 0,
    left: "0 !important",
    width: "100% !important",
  },
  "& .bm-item": {
    display: "inline-block",
    outline: "none",
  },
  "& .bm-overlay": {
    top: 0,
    // height: "100vh !important",
    left: 0,
    right: 0,
    backgroundColor: "rgb(255, 255, 255) !important",
    backdropFilter: "blur(4px)",
  },
  ".right a": {
    textAlign: "right",
  },
  ".bm-item .menu-wrap": {
    padding: "5px",
    display: "inline-block",
    width: "auto",
  },
  "& .right .bm-item .menu-wrap": {
    borderTopLeftRadius: border_radius,
    borderBottomLeftRadius: border_radius,
    borderTopRightRadius: 0,
    borderBottomRightRadius: 0,
    fontSize: font_size,
    paddingRight: 0,
    paddingLeft: menu_item_padding,
  },
  "& .bm-item .menu-wrap": {
    borderTopLeftRadius: 0,
    borderBottomLeftRadius: 0,
    borderTopRightRadius: border_radius,
    borderBottomRightRadius: border_radius,
    fontSize: font_size,
    paddingRight: menu_item_padding,
    paddingLeft: 0,
  },
  "& .icon-size": {
    fontSize: "19px",
  },
});

export default styles;
