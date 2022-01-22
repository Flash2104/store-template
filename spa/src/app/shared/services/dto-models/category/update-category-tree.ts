import { ICategoryTreeData } from './category-tree-data';

export interface IUpdateCategoryTreeRequest {
  tree: ICategoryTreeData;
  removedItemIds: number[] | null | undefined;
}

export interface IUpdateCategoryTreeResponse {
  tree: ICategoryTreeData;
}
