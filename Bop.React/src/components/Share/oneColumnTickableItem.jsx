/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { SelectableItem } from "../Share/selectable-item";
import { ThemeColor } from "./color";
import { PrimaryText } from "./text";
import { AiOutlineCheck } from "react-icons/ai";

const styles = css({});

export const OneColumnTickableItem = ({ text, active, onClick }) => {
  const renderCheckIcon = () => {
    const display = active ? "d-inline" : "d-none";
    return (
      <ThemeColor className={`${display} ml-1 mr-1`}>
        <AiOutlineCheck />
      </ThemeColor>
    );
  };

  return (
    <div css={styles}>
      <SelectableItem padding="p-2 font-small-1" rounded active={active} onClick={onClick}>
        {renderCheckIcon()}
        <PrimaryText className="d-inline-block">{text}</PrimaryText>
      </SelectableItem>
    </div>
  );
};
