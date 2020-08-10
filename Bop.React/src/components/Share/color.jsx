/** @jsx jsx */
import { jsx } from "@emotion/core";
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

export const ThemeBackgroundColor = ({ className, backgroundColor, color, children }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        backgroundColor: backgroundColor ? backgroundColor : theme.primaryColor,
        color: color,
      }}
    >
      {children}
    </div>
  );
};

export const ThemeColor = ({ className, children }) => {
  const theme = useTheme();
  return (
    <span
      className={className}
      css={{
        color: theme.primaryColor,
      }}
    >
      {children}
    </span>
  );
};

