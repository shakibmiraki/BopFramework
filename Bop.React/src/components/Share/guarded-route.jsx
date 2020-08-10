import React from "react";
import { Route, Redirect } from "react-router-dom";
import { userService } from "./../../services/user";
import { routes } from "../../constants/constant";

const hasAuthUserAccessToThisRoute = (permissions) => {
  console.log(permissions);
  if (!userService.isAuthUserLoggedIn()) {
    return false;
  }

  if (!permissions) {
    return true;
  }

  if (Array.isArray(permissions.deniedRoles) && Array.isArray(permissions.allowedRoles)) {
    throw new Error("Don't set both 'deniedRoles' and 'allowedRoles' in route data.");
  }

  if (Array.isArray(permissions.allowedRoles)) {
    const isInRole = userService.isAuthUserInRoles(permissions.allowedRoles);
    if (isInRole) {
      return true;
    }
    return false;
  }

  if (Array.isArray(permissions.deniedRoles)) {
    const isInRole = userService.isAuthUserInRoles(permissions.deniedRoles);
    if (!isInRole) {
      return true;
    }
    return false;
  }

  return true;
};

const GuardedRoute = ({ component: Component, permissions, ...rest }) => (
  <Route
    {...rest}
    render={(props) =>
      hasAuthUserAccessToThisRoute(permissions) ? <Component {...props} /> : <Redirect to={routes.login.base} />
    }
  />
);

export default GuardedRoute;
