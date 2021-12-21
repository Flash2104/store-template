import { IReferenceData } from './../reference-data';

export interface IMemberViewData {
  id: string | null | undefined;
  name: string | null | undefined;
  surname: string | null | undefined;
  city: string | null | undefined;
  avatar: string | null | undefined;
  isLeader: boolean | null | undefined;
  about: string | null | undefined;
  roles: IReferenceData<string>[] | null | undefined;
}

export interface ITeamData {
  id: string | null | undefined;
  title: string | null | undefined;
  city: string | null | undefined;
  foundationDate: string | null | undefined;
  avatar: string | null | undefined;
  teamRoles: IReferenceData<string>[] | null | undefined;
  members: IMemberViewData[] | null | undefined;
}
