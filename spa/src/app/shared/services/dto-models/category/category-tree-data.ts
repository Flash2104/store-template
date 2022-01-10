export interface ICategoryItemData {
  id: number;
  title: string;
  order: number;
  icon: string | null | undefined;
  isDisabled: string | null | undefined;
  children?: ICategoryItemData[] | null | undefined;
}

export interface ICategoryTreeData {
  id: number;
  title: string;
  isDefault?: boolean | null | undefined;
  items?: ICategoryItemData[] | null | undefined;
}
