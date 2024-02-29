import React from "react";
import { headers } from "next/headers";
import { clientWithAuth } from "@libs/APIClients";
import getCurrentUser from "@/actions/getCurrentUser";

async function getData(id) {
  const session = await getCurrentUser();
  const res = await fetch(`http://localhost:3000/api/shipments/${id}`, {
    method: "POST",
    headers: headers(),
    next: { revalidate: 10 },
    body: JSON.stringify({ Message: "" }),
  });

  //console.log("Shipment Request", res);

  if (!res.ok) {
    //throw new Error("Failed to fetch data");
    return {};
  }

  var jsonData = await res.json();
  //console.log(`Shipment ${id} Data`, jsonData);
  return jsonData.data;
}

export default async function ShipmentDetailsDashboard({ id }) {
  var shipment = await getData(id);
  //console.log(data);

  return (
    <>
      <div className="relative overflow-x-auto shadow-md sm:rounded-lg">
        <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
              <th scope="col" className="px-6 py-3">
                Shipment#
              </th>
              <th scope="col" className="px-6 py-3">
                {shipment.shipmentId}
              </th>
            </tr>
          </thead>
          <tbody>
            <tr className="bg-white border-b dark:bg-gray-900 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
              >
                Cnee:
              </th>
              <td className="px-6 py-4">
                {shipment.cneeCode &&
                  ` ${shipment.cneeCode}-${shipment.cneeName}`}
              </td>
            </tr>
            <tr className="border-b bg-gray-50 dark:bg-gray-800 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
              >
                Cnor:
              </th>
              <td className="px-6 py-4">
                {shipment.cnorCode &&
                  ` ${shipment.cnorCode}-${shipment.cnorCode}`}
              </td>
            </tr>
            <tr className="bg-white border-b dark:bg-gray-900 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
              >
                Goods Desc:
              </th>
              <td className="px-6 py-4">{shipment.goodsDesc}</td>
            </tr>
            <tr className="border-b bg-gray-50 dark:bg-gray-800 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
              >
                Routing:
              </th>
              <td className="px-6 py-4">
                {shipment.polCode && ` ${shipment.polCode}-${shipment.polName}`}
                =&gt;
                {shipment.podCode && ` ${shipment.podCode}-${shipment.podName}`}
              </td>
            </tr>
            <tr className="border-b bg-gray-50 dark:bg-gray-800 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                colSpan={2}
              >
                {" "}
              </th>
            </tr>
            <tr className="border-b bg-gray-50 dark:bg-gray-800 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                colSpan={2}
              >
                Milestone(s):
              </th>
            </tr>
            <tr className="border-b bg-gray-50 dark:bg-gray-800 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
              >
                Routing:
              </th>
              <td className="px-6 py-4"> </td>
            </tr>
          </tbody>
        </table>
      </div>
    </>
  );
}
