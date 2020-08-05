export class ResponseModel {
  messages: string[];
  result: ResultType;
}

export enum ResultType {
  Success = 0,
  Error = 1
}
