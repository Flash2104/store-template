import { IProfileAuthData } from '../profile-auth-data';
import { ITokenData } from '../token-data';
import { IUserData } from '../user-data';

export interface ISignInResponse {
  tokenData: ITokenData | null;
  user: IUserData | null;
  profile: IProfileAuthData | null;
}
