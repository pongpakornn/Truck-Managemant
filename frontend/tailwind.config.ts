import type { Config } from "tailwindcss";
const config: Config = { content: ["./src/**/*.{js,ts,jsx,tsx,mdx}"], theme: { extend: { colors: { nexus: { green: "#16794B", dark: "#252A2D", canvas: "#F4F6F5", line: "#E5E9E7" } }, boxShadow: { panel: "0 2px 10px rgba(16,24,40,.04)" } } }, plugins: [] };
export default config;
