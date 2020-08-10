import { spring } from "react-router-transition";

// we need to map the `scale` prop we define below
// to the transform style property
export function mapStyles(styles) {
  return {
    opacity: styles.opacity,
    filter: `blur(${styles.blur}px)`,
  };
}

// wrap the `spring` helper to use a bouncy config
function bounce(val) {
  return spring(val, {
    stiffness: 300,
    damping: 10,
  });
}

// child matches will...
export const bounceTransition = {
  // start in a transparent, upscaled state
  atEnter: {
    // opacity: 0,
    opacity: 0,
    blur: 8,
  },
  // leave in a transparent, downscaled state
  atLeave: {
    // opacity: bounce(0),
    opacity: 0,
    blur: bounce(8),
  },
  // and rest at an opaque, normally-scaled state
  atActive: {
    // opacity: bounce(1),
    opacity: 1,
    blur: bounce(0),
  },
};
