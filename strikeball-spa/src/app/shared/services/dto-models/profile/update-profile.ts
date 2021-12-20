import { IReferenceData } from '../reference-data';
import { IProfileData } from './profile-data';

export interface IUpdateProfileRequest {
  id: string | null | undefined;
  name: string | null | undefined;
  surname: string | null | undefined;
  birthDate: string | null | undefined;
  city: string | null | undefined;
  team: IReferenceData<string> | null | undefined;
}

export interface IUpdateProfileResponse {
  memberData: IProfileData | null;
}
