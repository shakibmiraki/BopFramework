/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx } from "@emotion/core";
import { Switch } from "react-router-dom";
import { Redirect } from "react-router-dom";
import { Route } from "react-router-dom";
import { routes } from "./../../constants/constant";
import { TransactionsFilterBasic } from "./filter/basic";
import { TransactionsFilterDate } from "./filter/date";
import { TransactionsFilterAmount } from './filter/amount';
import { TransactionsFilterStatus } from './filter/status';
import { TransactionsFilterType } from './filter/type';

export const TransactionsFilter = () => {
  return (
    <Switch>
      <Redirect from={routes.root} exact to={`${routes.transactions.base}${routes.transactions.filter}`} />
      <Route
        exact
        path={`${routes.transactions.base}${routes.transactions.filter}`}
        component={TransactionsFilterBasic}
      />
      <Route
        path={`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.date}`}
        component={TransactionsFilterDate}
      />
      <Route
        path={`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.amount}`}
        component={TransactionsFilterAmount}
      />
      <Route
        path={`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.status}`}
        component={TransactionsFilterStatus}
      />
      <Route
        path={`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.type}`}
        component={TransactionsFilterType}
      />
    </Switch>
  );
};
