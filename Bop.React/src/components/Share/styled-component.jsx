/** @jsx jsx */
import { jsx } from "@emotion/core";
import { colors } from "./../../constants/colors";
import { useTheme } from "emotion-theming";

export const Line = ({ className }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        width: "100%",
        height: "1px",
        backgroundColor: theme.primaryColor,
      }}
    ></div>
  );
};

export const CircleIcon = ({ className, children }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        color: theme.textColor,
        textAlign: "center",
        fontSize: "40px",
        border: `2px solid ${theme.textColor}`,
        borderRadius: "90px",
        width: "100px",
        height: "100px",
        margin: "auto",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: colors.white,
      }}
    >
      {children}
    </div>
  );
};

export const ThemeBorder = ({ className, borderThick, borderStyle, borderRadius = 0, children }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        border: `${borderThick}px ${borderStyle} ${theme.primaryColor}`,
        borderRadius: borderRadius,
      }}
    >
      {children}
    </div>
  );
};

export const ThemeBottomBorder = ({ className, borderThick, borderStyle, opacity = 1 }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        borderBottom: `${borderThick}px ${borderStyle} ${theme.primaryColor}`,
        opacity: opacity,
      }}
    ></div>
  );
};
