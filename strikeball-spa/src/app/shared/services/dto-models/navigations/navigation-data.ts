export interface INavigationItem {
  id: number;
  path: string;
  title: string;
  order: number;
  icon: string | null | undefined;
  disabled: string | null | undefined;
  children?: INavigationItem[] | null;
}

export interface IUserNavigationData {
  id: string;
  title: string;
  isDefault: boolean;
  navItems: INavigationItem[];
}

export interface INavigationDataResponse {
  navigations: IUserNavigationData[] | null | undefined;
}
