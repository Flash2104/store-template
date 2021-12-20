export interface IReferenceData<T> {
  id: T;
  title: string;
  grade: number | null;
}

export interface IGroupedReferenceData<T> {
  key: string;
  data: IReferenceData<T>[];
}
