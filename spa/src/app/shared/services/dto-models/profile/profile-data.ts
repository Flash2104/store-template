import { IReferenceData } from './../reference-data';
export interface IProfileData {
  id: string | null | undefined;
  name: string | null | undefined;
  surname: string | null | undefined;
  about: string | null | undefined;
  birthDate: string | null | undefined;
  city: string | null | undefined;
  avatarData: string | null | undefined;
  avatarIcon: string | null | undefined;
  email: string | null | undefined;
  phone: string | null | undefined;
  team: IReferenceData<string> | null | undefined;
  roles: IReferenceData<string>[] | null | undefined;
}
