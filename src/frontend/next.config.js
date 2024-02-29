/** @type {import('next').NextConfig} */
const nextConfig = {
  experimental: {
    serverActions: true,
  },
  images: {
    domains: ["flowbite.s3.amazonaws.com"],
  },
  reactStrictMode: false,
};

module.exports = nextConfig;
