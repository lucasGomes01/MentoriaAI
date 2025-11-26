import axios from "axios";
//import { supabase } from "../auth/supabase";

export const api = axios.create({
  baseURL: "http://localhost:5000", // API Gateway
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("access_token");
  //const token = supabase.auth.getSession();

  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});
