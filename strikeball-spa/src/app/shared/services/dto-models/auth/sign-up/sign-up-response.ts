import { IProfileAuthData } from './../profile-auth-data';
import { ITokenData } from '../token-data';
import { IUserData } from '../user-data';

export interface ISignUpResponse {
  tokenData: ITokenData | null;
  user: IUserData | null;
  profile: IProfileAuthData | null;
}
