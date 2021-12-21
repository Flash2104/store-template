export type OperationCode =
  | 'Success'
  | 'CallFailed'
  | 'OperationFailed'
  | 'IncorrentConditions'
  | 'AssertFailed'
  | 'UnAuthorized';

export interface IServerError {
  code: number;
  message: string;
}

export interface IOperationStatus {
  code: OperationCode | null;
  error: IServerError;
}

export interface IServerResponse<T> {
  data: T | null;
  isSuccess: boolean;
  errors: IServerError[] | null;
}
