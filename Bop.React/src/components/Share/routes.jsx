import React from "react";
// import { AnimatedSwitch } from "react-router-transition";
import { Redirect, Route, Router, Switch } from "react-router-dom";
import { bounceTransition, mapStyles } from "./routes-animate";
import { routes } from "./../../constants/constant";
import history from "./../../services/history";
import Splash from "../../pages/splash/index";
import Language from "../../pages/language/index";
import Home from "../../pages/home/index";
import SignUp from "./../../pages/sign-up/index";
import Profile from "./../../pages/profile/index";
import NotFound from "../../pages/not-found";
import AboutUs from "./../../pages/about-us/index";
import Login from "./../../pages/login/index";
import GuardedRoute from "./guarded-route";

const Routing = () => {
  return (
    <Router history={history}>
      <Switch
        atEnter={bounceTransition.atEnter}
        atLeave={bounceTransition.atLeave}
        atActive={bounceTransition.atActive}
        mapStyles={mapStyles}
        className="switch-wrapper"
      >
        <Route path={routes.splash} component={Splash} />
        <Route path={routes.language} component={Language} />
        {/* <Route path={routes.home} component={Home} /> */}
        <Route path={routes.sign_up.base} component={SignUp} />
        <Route path={routes.login.base} component={Login} />
        <GuardedRoute path={routes.home} permissions={{ allowedRoles: ["Registered"] }} component={Home} />
        <GuardedRoute path={routes.profile} permissions={{ allowedRoles: ["Registered"] }} component={Profile} />
        <GuardedRoute path={routes.about_us} permissions={{ allowedRoles: ["Registered"] }} component={AboutUs} />
        <Route path={routes.not_found} component={NotFound} />
        <Redirect exact from={routes.root} to={routes.splash} />
        <Redirect to={routes.not_found} />
      </Switch>
    </Router>
  );
};

export default Routing;
