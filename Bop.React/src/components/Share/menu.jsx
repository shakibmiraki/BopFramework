/** @jsx jsx */
import { jsx } from "@emotion/core";
import { colors } from "./../../constants/colors";
import { useTheme } from "emotion-theming";

export const MenuItem = ({ className, children, active }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        transition: "0.3s all",
        backgroundColor: active ? theme.primaryColor : "",
        color: active ? colors.white : colors.gray,
      }}
    >
      {children}
    </div>
  );
};
