/** @jsx jsx */
import { jsx, css } from "@emotion/core";

const styles = css({
  maxWidth: "375px",
  margin: "auto",
  ".label": {
    fontSize: "14px",
  },
});

export const Card = ({ className, children }) => {
  return (
    <div className={className} css={styles} id="share-as-image">
      <div className="card shadow-sm">
        <div className="card-body">{children}</div>
      </div>
    </div>
  );
};
