export interface ICitiesData {
  id: number;
  city: string;
  cityAddress: string;
  federalDistrict: string | null | undefined;
}

export interface IRegionData {
  cities: ICitiesData[] | null;
  title: string;
  id: number;
}

export interface IGetCityReferencesResponse {
  regions: IRegionData[] | null;
}

