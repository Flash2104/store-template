export interface ICategoryItemData {
  id: number;
  title: string;
  order: number;
  icon: string | null | undefined;
  isDisabled: string | null | undefined;
  children?: ICategoryItemData[] | null | undefined;
}

export interface ICategoryTreeData {
  id: string;
  title: string;
  isDefault: boolean;
  items: ICategoryItemData[] | null | undefined;
}
