export interface ICategoryItemData {
  id?: number | null | undefined;
  title?: string | null | undefined;
  order: number;
  icon: string | null | undefined;
  isDisabled: boolean | null | undefined;
  children?: ICategoryItemData[] | null | undefined;
}

export interface ICategoryTreeData {
  id: number;
  title: string;
  isDefault?: boolean | null | undefined;
  items?: ICategoryItemData[] | null | undefined;
}
