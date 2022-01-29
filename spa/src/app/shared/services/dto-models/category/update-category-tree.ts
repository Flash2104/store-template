export interface IUpdateCategoryItemData {
  id?: number | null | undefined;
  tempId?: string | null | undefined;
  title?: string | null | undefined;
  order: number;
  icon: string | null | undefined;
  isDisabled: boolean | null | undefined;
  children?: IUpdateCategoryItemData[] | null | undefined;
}

export interface IUpdateCategoryTreeData {
  id: number;
  title: string;
  isDefault?: boolean | null | undefined;
  items?: IUpdateCategoryItemData[] | null | undefined;
}

export interface IUpdateCategoryTreeRequest {
  tree: IUpdateCategoryTreeData;
  removedItemIds: number[] | null | undefined;
}

export interface IUpdateCategoryTreeResponse {
  tree: IUpdateCategoryTreeData;
}
