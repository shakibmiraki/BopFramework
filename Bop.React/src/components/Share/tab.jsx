/** @jsx jsx */
import { jsx } from "@emotion/core";
import { colors } from "./../../constants/colors";
import { useTheme } from "emotion-theming";

export const MenuTab = ({ className, active, children }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        padding: "3px",
        transition: "0.3s all",
        maxHeight: "55px",
        color: active ? theme.primaryColor : colors.gray,
        borderBottom: 0,
      }}
    >
      {children}
    </div>
  );
};
