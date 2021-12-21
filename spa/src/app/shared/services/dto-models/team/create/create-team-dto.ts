import { ITeamData } from '../team-data';

export interface ICreateTeamRequest {
  cityId: number | null | undefined;
  title: string;
  foundationDate: string | null | undefined;
  avatar: string | null | undefined;
}

export interface ICreateTeamResponse {
  teamData: ITeamData | null;
}
