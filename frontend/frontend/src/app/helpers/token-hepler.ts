import { jwtDecode } from 'jwt-decode';

export interface JwtPayload {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"?: string;
  role?: string;
  exp?: number;
}

export class TokenHelper {
  private static getToken(): string | null {
    return localStorage.getItem('jwt');
  }

  static getUserId(): number | null {
    const token = localStorage.getItem('jwt')?.replace('Bearer ', '');
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      console.log('caoo');
      const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      return userId ? +userId : null; 
    } catch (err) {
      console.error('JWT decode error:', err);
      return null;
    }
  }

  static getRole(): string | null {
    const token = this.getToken();
    if (!token) return null;
    console.log('token:', token);
    const decoded = jwtDecode<JwtPayload>(token);
    return decoded.role ?? null;
  }

  static isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    const decoded = jwtDecode<JwtPayload>(token);
    const currentTime = Math.floor(Date.now() / 1000);
    return (decoded.exp ?? 0) < currentTime;
  }
}
